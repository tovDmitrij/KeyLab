import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
//import { authApiService, baseQueryWithReauth } from "./userService";

import { getBaseQuery } from "./getBaseQuery";
import { API_URL } from "../constants";

type TPages = {
  /**
   * Номер страницы
   */
  page?: number;
  /**
   * Размер страницы
   */
  pageSize?: number;
};

export const keyboardsService = createApi({
  reducerPath: "keyboardsService",
  baseQuery: getBaseQuery,
  endpoints: (builder) => ({
    getAuthKeyboards: builder.query<any, TPages>({
      query: ({ page, pageSize }) => ({
        url: `/keyboards/auth?page=${page}&pageSize=${pageSize}`,
        method: "GET",
      }),
    }),
    getAuthKeyboardsTotalPages: builder.query<any, TPages>({
      query: ({ pageSize }) => ({
        url: `keyboards/auth/totalPages?pageSize=${pageSize}`,
        method: "GET",
      }),
    }),
    getDefaultKeyboards: builder.query<any, TPages>({
      query: ({ page, pageSize }) => ({
        url: `/keyboards/default?page=${page}&pageSize=${pageSize}`,
        method: "GET",
      }),
    }),
    getDefaultKeyboardsTotalPages: builder.query<any, TPages>({
      query: ({ pageSize }) => ({
        url: `/keyboards/default/totalPages?pageSize=${pageSize}`,
        method: "GET",
      }),
    }),
  }),
});

export const {
  useGetAuthKeyboardsQuery,
  useLazyGetAuthKeyboardsQuery,
  useGetAuthKeyboardsTotalPagesQuery,
  useLazyGetAuthKeyboardsTotalPagesQuery,
  useGetDefaultKeyboardsQuery,
  useLazyGetDefaultKeyboardsQuery,
  useGetDefaultKeyboardsTotalPagesQuery,
  useLazyGetDefaultKeyboardsTotalPagesQuery
} = keyboardsService;
