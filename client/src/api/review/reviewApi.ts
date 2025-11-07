import type { Review, ReviewInput } from './types/review';

import { routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';

import { http } from '../common/http';
import { getAuthConfig, returnIfRequestCanceled } from '../common/utils';


export async function all(
  bookId: number,
  pageIndex: number,
  pageSize: number,
  token: string,
  signal?: AbortSignal,
) {
  try {
    const url = `${routes.review}/${bookId}?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    const response = await http.get<{ items: Review[]; totalItems: number }>(
      url,
      getAuthConfig(token, signal),
    );

    return response.data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.review.list);
    throw error;
  }
}

export async function create(review: ReviewInput, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.review}`;
    const response = await http.post<any>(url, review, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.review.create);
    throw error;
  }
}

export async function edit(id: number, review: ReviewInput, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.review}/${id}`;
    await http.put(url, review, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    returnIfRequestCanceled(error, errors.review.edit);
    throw error;
  }
}

export async function remove(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.review}/${id}`;
    await http.delete(url, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    returnIfRequestCanceled(error, errors.review.delete);
    throw error;
  }
}

export async function upvote(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.vote}`;
    const vote: any = { reviewId: id, isUpvote: true };

    await http.post(url, vote, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    if ((error as any)?.name === 'CanceledError') {
      throw error;
    }

    return false;
  }
}

export async function downvote(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.vote}`;
    const vote: any = { reviewId: id, isUpvote: false };

    await http.post(url, vote, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    if ((error as any)?.name === 'CanceledError') {
      throw error;
    }

    return false;
  }
}
