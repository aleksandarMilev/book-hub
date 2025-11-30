import type {
  Author,
  AuthorDetails,
  AuthorDetailsDto,
  AuthorNames,
  CreateAuthor,
} from '@/features/author/types/author.js';
import { genderFromServer, GenderToServer } from '@/features/author/types/author.js';
import {
  getAuthConfig,
  getAuthConfigForFile,
  getPublicConfig,
  http,
  httpAdmin,
  processError,
} from '@/shared/api/http.js';
import { routes } from '@/shared/lib/constants/api.js';
import { baseErrors, errors } from '@/shared/lib/constants/errorMessages.js';

export const names = async (token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.authorNames}`;
    const { data } = await http.get<AuthorNames[]>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    processError(error, errors.author.all);
  }
};

export const topThree = async (signal?: AbortSignal) => {
  try {
    const url = `${routes.topThreeAuthors}`;
    const { data } = await http.get<Author[]>(url, getPublicConfig(signal));

    return data;
  } catch (error) {
    processError(error, errors.author.topThree);
  }
};

export async function details(id: string, token: string, isAdmin: boolean, signal?: AbortSignal) {
  try {
    const url = `${routes.author}/${id}`;
    const httpClient = isAdmin ? httpAdmin : http;
    const { data } = await httpClient.get<AuthorDetailsDto>(url, getAuthConfig(token, signal));

    return mapAuthorDetailsDto(data);
  } catch (error) {
    processError(error, errors.author.byId);
  }
}

export const create = async (author: CreateAuthor, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.author}`;
    const formData = new FormData();
    writeFormData(formData, author);

    const { data } = await http.post<AuthorDetails>(
      url,
      formData,
      getAuthConfigForFile(token, signal),
    );

    return mapAuthorDetailsDto(data as unknown as AuthorDetailsDto);
  } catch (error) {
    processError(error, errors.author.create);
  }
};

export const edit = async (
  id: string,
  author: CreateAuthor,
  token: string,
  signal?: AbortSignal,
) => {
  try {
    const url = `${routes.author}/${id}`;
    const formData = new FormData();

    writeFormData(formData, author);
    await http.put(url, formData, getAuthConfigForFile(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.author.edit);
  }
};

export const remove = async (id: string, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.author}/${id}`;
    await http.delete(url, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.author.delete);
  }
};

export const approve = async (id: string, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.author}/${id}/approve`;
    await httpAdmin.patch<void>(url, null, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, baseErrors.general);
  }
};

export const reject = async (id: string, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.author}/${id}/reject`;
    await httpAdmin.patch<void>(url, null, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, baseErrors.general);
  }
};

const mapAuthorDetailsDto = (dto: AuthorDetailsDto): AuthorDetails => ({
  ...dto,
  gender: genderFromServer(dto.gender),
});

const writeFormData = (formData: FormData, author: CreateAuthor) => {
  formData.append('name', author.name);
  formData.append('biography', author.biography);

  if (author.penName) {
    formData.append('penName', author.penName);
  }

  if (author.image) {
    formData.append('image', author.image);
  }

  if (author.gender) {
    formData.append('gender', String(GenderToServer[author.gender]));
  }

  if (author.nationality != null) {
    formData.append('nationality', String(author.nationality));
  }

  if (author.bornAt) {
    formData.append('bornAt', author.bornAt);
  }

  if (author.diedAt) {
    formData.append('diedAt', author.diedAt);
  }
};
