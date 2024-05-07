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

type TSwitches = {
  /**
   * id свитча
   */
  id?: string;
  /**
   * название свитча
   */
  title?: string;
};


export const switchesService = createApi({
  reducerPath: "switchesService",
  baseQuery: getBaseQuery,
  endpoints: (builder) => ({
    getSwitches: builder.query<TSwitches[], TPages>({
      query: ({ page, pageSize }) => ({
        url: `/switches/default?page=${page}&pageSize=${pageSize}`,
        method: "GET",
      }),
    }),
    getSwitchesTotalPages: builder.query<any, TPages>({
      query: ({ pageSize }) => ({
        url: `/switches/default/totalPages?pageSize=${pageSize}`,
        method: "GET",
      }),
    }),
    getSwitch: builder.query<string, string>({
      query: (ID : string) => ({
        url: `/switches/file?switchID=${ID}`,
        method: "GET",
        responseHandler: response => response.arrayBuffer()
      }),
    }),
    getSwitchPreview: builder.query<any, string>({
      query: (ID : string ) => ({
        url: `/switches/preview?switchID=${ID}`,
        method: "GET",
        responseHandler: response => response.blob()
      }),
    }),
    getSwitchSound: builder.query<any, string>({
      query: (ID : string ) => ({
        url: `/switches/sound?switchID=${ID}`,
        method: "GET",
      }),
    }),
  }),
});

export const {
  useGetSwitchPreviewQuery,
  useGetSwitchQuery,
  useGetSwitchesTotalPagesQuery,
  useGetSwitchSoundQuery,
  useGetSwitchesQuery,
  useLazyGetSwitchPreviewQuery,
  useLazyGetSwitchQuery,
  useLazyGetSwitchSoundQuery,
  useLazyGetSwitchesQuery,
  useLazyGetSwitchesTotalPagesQuery
} = switchesService;
