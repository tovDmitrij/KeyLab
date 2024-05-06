import { createApi } from "@reduxjs/toolkit/query/react";

import { getBaseQuery } from "./getBaseQuery";

export const statsService = createApi({
  reducerPath: "statsService",
  baseQuery: getBaseQuery,
  endpoints: (builder) => ({
    averageTime: builder.mutation<any, any>({
      query: (body) => ({
        url: `stats/attendance/time/plot`,
        method: "POST",
        body: body,
      }),
    }),
    countUsers: builder.mutation<any, any>({
      query: (body) => ({
        url: `stats/attendance/quantity/plot`,
        method: "POST",
        body: body,
      }),
    }),
    averageTimeOnPages: builder.mutation<any, any>({
      query: (body) => ({
        url: `stats/activities/time/plot`,
        method: "POST",
        body: body,
      }),
    }),
    countUsersOnPages: builder.mutation<any, any>({
      query: (body) => ({
        url: `stats/activities/quantity/plot`,
        method: "POST",
        body: body,
      }),
    }),
    averageTimeAtom: builder.mutation<any, any>({
      query: (body) => ({
        url: `stats/attendance/time/atom`,
        method: "POST",
        body: body,
      }),
    }),
    countUsersAtom: builder.mutation<any, any>({
      query: (body) => ({
        url: `stats/attendance/quantity/atom`,
        method: "POST",
        body: body,
      }),
    }),
    averageTimeOnPagesAtom: builder.mutation<any, any>({
      query: (body) => ({
        url: `stats/activities/time/atom`,
        method: "POST",
        body: body,
      }),
    }),
    countUsersOnPagesAtom: builder.mutation<any, any>({
      query: (body) => ({
        url: `stats/activities/quantity/atom`,
        method: "POST",
        body: body,
      }),
    }),
    getActivites: builder.query<any, void>({
      query: (body) => ({
        url: `stats/activities`,
        method: "GET",
        body: body,
      }),
    }),
    getIntervals: builder.query<any, void>({
      query: (body) => ({
        url: `stats/intervals`,
        method: "GET",
        body: body,
      }),
    }),
  }),
});

export const {
  useAverageTimeAtomMutation,
  useAverageTimeOnPagesAtomMutation,
  useCountUsersAtomMutation,
  useCountUsersOnPagesAtomMutation,
  useCountUsersOnPagesMutation,
  useAverageTimeOnPagesMutation,
  useCountUsersMutation,
  useAverageTimeMutation,
  useGetActivitesQuery,
  useGetIntervalsQuery,
} = statsService;
