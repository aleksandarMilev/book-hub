import type { CreateReview, Review, VoteRequest } from '@/features/review/types/review.js';
import { getAuthConfig, http, processError } from '@/shared/api/http.js';
import { routes } from '@/shared/lib/constants/api.js';
import { errors } from '@/shared/lib/constants/errorMessages.js';
import type { PaginatedResult } from '@/shared/types/paginatedResult.js';

export async function all(
  bookId: number,
  pageIndex: number,
  pageSize: number,
  token: string,
  signal?: AbortSignal,
) {
  try {
    const url = `${routes.review}/${bookId}?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    const response = await http.get<PaginatedResult<Review>>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    processError(error, errors.review.all);
  }
}

export async function create(review: CreateReview, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.review}`;
    const response = await http.post(url, review, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    processError(error, errors.review.create);
  }
}

export async function edit(id: number, review: CreateReview, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.review}/${id}`;
    await http.put(url, review, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.review.edit);
  }
}

export async function remove(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.review}/${id}`;
    await http.delete(url, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.review.delete);
  }
}

export async function upvote(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.vote}`;
    const vote = { reviewId: id, isUpvote: true };
    await http.post(url, vote, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.review.vote.up);
  }
}

export async function downvote(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.vote}`;
    const vote: VoteRequest = { reviewId: id, isUpvote: false };
    await http.post(url, vote, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.review.vote.down);
  }
}
