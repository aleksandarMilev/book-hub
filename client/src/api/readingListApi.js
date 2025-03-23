import { baseUrl, routes } from "../common/constants/api";
import { errors } from "../common/constants/messages";

export async function getAsync(userId, token, status, page, pageSize) {
  const options = {
    method: "GET",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  };

  const urlParamObject = {
    userId,
    status,
  };

  if (page) {
    urlParamObject.pageIndex = page;
  }
  if (pageSize) {
    urlParamObject.pageSize = pageSize;
  }

  const params = new URLSearchParams(urlParamObject);
  const url = `${baseUrl + routes.readingList}?${params.toString()}`;

  const response = await fetch(url, options);

  if (response.ok) {
    return await response.json();
  }

  throw new Error(errors.readingList.currentlyReading);
}

export async function addInListAsync(bookId, status, token) {
  const bodyObj = {
    bookId,
    status,
  };

  const options = {
    method: "POST",
    headers: {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json",
    },
    body: JSON.stringify(bodyObj),
  };

  const url = baseUrl + routes.readingList;
  const response = await fetch(url, options);

  if (response.status === 400) {
    return await response.json();
  }

  if (!response.ok) {
    throw new Error(errors.readingList.add);
  }

  return null;
}

export async function removeFromListAsync(bookId, status, token) {
  const bodyObj = {
    bookId,
    status,
  };

  const options = {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json",
    },
    body: JSON.stringify(bodyObj),
  };

  const url = baseUrl + routes.readingList;
  const response = await fetch(url, options);

  if (!response.ok) {
    throw new Error(errors.readingList.add);
  }
}
