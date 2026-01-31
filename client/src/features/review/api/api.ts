import type { CreateReview, Review, VoteRequest } from '@/features/review/types/review';
import { getAuthConfig, http, processError } from '@/shared/api/http';
import { routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';
import type { PaginatedResult } from '@/shared/types/paginatedResult';

export async function all(
  bookId: string,
  pageIndex: number,
  pageSize: number,
  token: string,
  signal?: AbortSignal,
) {
  try {
    const url = `${routes.review}/book/${bookId}?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    const { data } = await http.get<PaginatedResult<Review>>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    processError(error, errors.review.all);
  }
}

export async function create(review: CreateReview, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.review}`;
    const { data } = await http.post(url, review, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    processError(error, errors.review.create);
  }
}

export async function edit(id: string, review: CreateReview, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.review}/${id}`;
    await http.put(url, review, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.review.edit);
  }
}

export async function remove(id: string, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.review}/${id}`;
    await http.delete(url, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.review.delete);
  }
}

export async function upvote(id: string, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.vote}`;
    const vote = { reviewId: id, isUpvote: true };
    await http.post(url, vote, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.review.vote.up);
  }
}

export async function downvote(id: string, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.vote}`;
    const vote: VoteRequest = { reviewId: id, isUpvote: false };
    await http.post(url, vote, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.review.vote.down);
  }
}


