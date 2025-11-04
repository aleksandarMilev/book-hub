import axios from 'axios';
import { baseUrl, routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';
import type { CreateReview } from './types/createReview';
import { getAuthConfig } from '../common/utils';
import type { Review } from './types/review';
import type { VoteRequest } from './types/voteRequest';

export async function create(review: CreateReview, token: string) {
  try {
    const url = `${baseUrl}${routes.review}`;
    const response = await axios.post<Review>(url, review, getAuthConfig(token));

    return response.data;
  } catch {
    throw new Error(errors.review.create);
  }
}

export async function edit(id: number, review: CreateReview, token: string) {
  try {
    const url = `${baseUrl}${routes.review}/${id}`;
    await axios.put(url, review, getAuthConfig(token));

    return true;
  } catch {
    throw new Error(errors.review.edit);
  }
}

export async function remove(id: number, token: string) {
  try {
    const url = `${baseUrl}${routes.review}/${id}`;
    await axios.delete(url, getAuthConfig(token));

    return true;
  } catch {
    return false;
  }
}

export async function upvote(id: number, token: string) {
  try {
    const url = `${baseUrl}${routes.vote}`;
    const vote: VoteRequest = { reviewId: id, isUpvote: true };

    await axios.post(url, vote, getAuthConfig(token));

    return true;
  } catch {
    return false;
  }
}

export async function downvote(id: number, token: string) {
  try {
    const url = `${baseUrl}${routes.vote}`;
    const vote: VoteRequest = { reviewId: id, isUpvote: false };

    await axios.post(url, vote, getAuthConfig(token));

    return true;
  } catch {
    return false;
  }
}
