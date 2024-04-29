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
    getAuthKitsTotalPages: builder.query<any, TPages>({
      query: ({ pageSize }) => ({
        url: `kits/auth/totalPages?pageSize=${pageSize}`,
        method: "GET",
      }),
    }),
    getDefaultKits: builder.query<TKits[], TPages>({
      query: ({ page, pageSize, typeID }) => ({
        url: `/kits/default?page=${page}&pageSize=${pageSize}&boxTypeID=${typeID}`,
        method: "GET",
      }),
    }),
    getDefaultKitsTotalPages: builder.query<any, TPages>({
      query: ({ pageSize }) => ({
        url: `/kits/default/totalPages?pageSize=${pageSize}`,
        method: "GET",
      }),
    }),
    postKits: builder.mutation<any, string>({
      query: (title) => ({
        url: `/kits`,
        method: "POST",
        body:  title,
      }),
    }),
  }),
});

export const {
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
