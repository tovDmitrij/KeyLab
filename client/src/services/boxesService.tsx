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
  typeID?: string;
};

type TBoxes = {
  /**
   * id бокса
   */
  id?: string;
  /**
   * название бокса
   */
  title?: string;

  /**
   * id типа бокса
   */
  typeID?: string;

  /**
   * название типа бокса
   */
  typeTitle?: string;

  /**
   * дата создания бокса
   */
  creationDate?: string;
};

export const boxesService = createApi({
  reducerPath: "boxesService",
  baseQuery: getBaseQuery,
  endpoints: (builder) => ({
    getDefaultBoxes: builder.query<TBoxes[], TPages>({
      query: ({ page, pageSize, typeID }) => ({
        url: `/boxes/default?page=${page}&pageSize=${pageSize}&typeID=${typeID}`,
        method: "GET",
      }),
    }),
    getDefaultTotalPages: builder.query<any, TPages>({
      query: ({ pageSize }) => ({
        url: `/boxes/default/totalPages?pageSize=${pageSize}`,
        method: "GET",
      }),
    }),
    getAuthBoxes: builder.query<TBoxes[], TPages>({
      query: ({ page, pageSize, typeID }) => ({
        url: `/boxes/auth?page=${page}&pageSize=${pageSize}&typeID=${typeID}`,
        method: "GET",
      }),
    }),
    getAuthBoxesTotalPages: builder.query<any, TPages>({
      query: ({ pageSize }) => ({
        url: `/boxes/auth/totalPages?pageSize=${pageSize}`,
        method: "GET",
      }),
    }),
    getBoxes: builder.query<string, string>({
      query: (ID: string) => ({
        url: `/boxes/file?boxID=${ID}`,
        method: "GET",
        responseHandler: (response) => response.arrayBuffer(),
      }),
    }),
    postBox: builder.mutation<any, any>({
      query: (bodyFormData) => {
        let formData = new FormData();
        formData.append('file', bodyFormData.file);
        formData.append('preview', bodyFormData.preview);
        formData.append('title', bodyFormData.title);
        formData.append('typeID', bodyFormData.typeID); 
        return {
          url: `/boxes`,
          method: "POST",
          body:  formData,
          formData: true,
        };
      },
    }),
  }),
});

export const {
  usePostBoxMutation,
  useLazyGetAuthBoxesQuery,
  useLazyGetAuthBoxesTotalPagesQuery,
  useGetAuthBoxesQuery,
  useGetAuthBoxesTotalPagesQuery,
  useGetBoxesQuery,
  useGetDefaultBoxesQuery,
  useGetDefaultTotalPagesQuery,
  useLazyGetBoxesQuery,
  useLazyGetDefaultBoxesQuery,
  useLazyGetDefaultTotalPagesQuery,
} = boxesService;
