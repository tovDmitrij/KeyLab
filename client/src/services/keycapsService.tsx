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

  /**
   * id типа 
   */
  kitID?: string;
};

type TKeycaps = {
  /**
   * id бокса
   */
  id?: string;
  /**
   * название бокса
   */
  title?: string;

  /**
   * дата создания бокса
   */
  creationDate?: string;
};

export const keycapsService = createApi({
  reducerPath: "keycapsService",
  baseQuery: getBaseQuery,
  endpoints: (builder) => ({
    getKeycaps: builder.query<TKeycaps[], TPages>({
      query: ({ page, pageSize, kitID }) => ({
        url: `/keycaps?page=${page}&pageSize=${pageSize}&kitID=${kitID}`,
        method: "GET",
      }),
    }),
    getKeycapsTotalPages: builder.query<any, TPages>({
      query: ({ pageSize, kitID }) => ({
        url: `/keycaps/totalPages?pageSize=${pageSize}&kitID=${kitID}`,
        method: "GET",
      }),
    }),
    getKeycap: builder.query<string, string>({
      query: (ID: string) => ({
        url: `/keycaps/file?keycapID=${ID}`,
        method: "GET",
        responseHandler: (response) => response.arrayBuffer(),
      }),
    }),
    postKeycap: builder.mutation<any, any>({
      query: (bodyFormData) => {
        let formData = new FormData();
        formData.append('file', bodyFormData.file);
        formData.append('preview', bodyFormData.preview);
        formData.append('title', bodyFormData.title);
        formData.append('kitID', bodyFormData.kitID); 
        return {
          url: `/keycaps`,
          method: "POST",
          body:  formData,
          formData: true,
        };
      },
    }),
  }),
});

export const {
  useGetKeycapsQuery,
  useLazyGetKeycapsQuery,
  useGetKeycapsTotalPagesQuery,
  useLazyGetKeycapsTotalPagesQuery,
  usePostKeycapMutation,
  useGetKeycapQuery,
  useLazyGetKeycapQuery,
} = keycapsService;
