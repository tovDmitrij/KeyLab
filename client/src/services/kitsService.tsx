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

type TKits = {
  /**
   * id кита
   */
  id?: string;
  /**
   * название кита
   */
  title?: string;
  /**
   * дата создания бокса
   */
  creationDate?: string;
};

export const kitsService = createApi({
  reducerPath: "kitsService",
  baseQuery: getBaseQuery,
  endpoints: (builder) => ({
    getAuthKits: builder.query<TKits[], TPages>({
      query: ({ page, pageSize, typeID }) => ({
        url: `/kits/auth?page=${page}&pageSize=${pageSize}&boxTypeID=${typeID}`,
        method: "GET",
      }),
    }),
    getDefaultKits: builder.query<TKits[], TPages>({
      query: ({ page, pageSize, typeID }) => ({
        url: `/kits/default?page=${page}&pageSize=${pageSize}&boxTypeID=${typeID}`,
        method: "GET",
      }),
    }),
    getAuthKitsTotalPages: builder.query<any, TPages>({
      query: ({ pageSize }) => ({
        url: `kits/auth/totalPages?pageSize=${pageSize}`,
        method: "GET",
      }),
    }),
    getDefaultKitsTotalPages: builder.query<any, TPages>({
      query: ({ pageSize }) => ({
        url: `/kits/default/totalPages?pageSize=${pageSize}`,
        method: "GET",
      }),
    }),
    postKits: builder.mutation<any, any>({
      query: (bodyFormData) => {
      let formData = new FormData();
      formData.append('title', bodyFormData.title);
      formData.append('boxTypeID', bodyFormData.boxTypeID); 
      return {
        url: `/kits`,
        method: "POST",
        body:  formData,
        formData: true,
      }},
    }),
    getKitPreview: builder.query<any, string>({
      query: (ID : string ) => ({
        url: `/kits/preview?kitID=${ID}`,
        method: "GET",
      }),
    }),
    patchKitsPreview: builder.mutation<any, any>({
      query: (bodyFormData) => {
      let formData = new FormData();
      formData.append('kitID', bodyFormData.kitID);
      formData.append('preview', bodyFormData.preview); 
      return {
        url: `kits/preview`,
        method: "PATCH",
        body:  formData,
        formData: true,
      }},
    }),
  }),
});

export const {
  useGetKitPreviewQuery,
  useLazyGetKitPreviewQuery,
  usePatchKitsPreviewMutation,
  useGetAuthKitsQuery,
  useGetAuthKitsTotalPagesQuery,
  useGetDefaultKitsQuery,
  useGetDefaultKitsTotalPagesQuery,
  useLazyGetAuthKitsQuery,
  useLazyGetAuthKitsTotalPagesQuery,
  useLazyGetDefaultKitsQuery,
  useLazyGetDefaultKitsTotalPagesQuery,
  usePostKitsMutation
} = kitsService;
