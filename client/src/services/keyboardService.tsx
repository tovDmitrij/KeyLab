import { createApi } from "@reduxjs/toolkit/query/react";

import { getBaseQuery } from "./getBaseQuery";

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
    getKeyBoard: builder.query<any, string>({
      query: (ID : string ) => ({
        url: `keyboards/file?keyboardID=${ID}`,
        method: "GET",
      }),
    }),
    getKeyBoardPreview: builder.query<any, string>({
      query: (ID : string ) => ({
        url: `keyboards/preview?keyboardID=${ID}`,
        method: "GET",
      }),
    }),
    postKeyboard: builder.mutation<any, any>({
      query: (bodyFormData) => {
        let formData = new FormData();
        formData.append('file', bodyFormData.file);
        formData.append('preview', bodyFormData.preview);
        formData.append('title', bodyFormData.title);
        formData.append('switchTypeID', bodyFormData.switchTypeID);
        formData.append('boxTypeID', bodyFormData.boxTypeID);

        return {
          url: `/keyboards`,
          method: "POST",
          body:  formData,
          formData: true,
        };
      },
    }),
  }),
});

export const {
  useLazyGetKeyBoardQuery,
  useGetKeyBoardQuery,
  useGetKeyBoardPreviewQuery,
  useLazyGetKeyBoardPreviewQuery,
  usePostKeyboardMutation,
  useGetAuthKeyboardsQuery,
  useLazyGetAuthKeyboardsQuery,
  useGetAuthKeyboardsTotalPagesQuery,
  useLazyGetAuthKeyboardsTotalPagesQuery,
  useGetDefaultKeyboardsQuery,
  useLazyGetDefaultKeyboardsQuery,
  useGetDefaultKeyboardsTotalPagesQuery,
  useLazyGetDefaultKeyboardsTotalPagesQuery
} = keyboardsService;
