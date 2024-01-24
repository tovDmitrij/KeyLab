import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { API_URL } from "../constants";

export const userService = createApi({
  reducerPath: "userService",
  baseQuery: fetchBaseQuery({ baseUrl: API_URL }),
  endpoints: (builder) => ({
    verifEmail: builder.mutation({
      query: (body) => ({
        url: "verifications/email",
        method: "POST",
        body: body,
        responseHandler: (response) => response.text(),
      }),
    }),
    signUp: builder.mutation({
      query: (body) => ({
        url: "users/signUp",
        method: "POST",
        body: body,
        responseHandler: (response) => response.text(),
      }),
    }),
  }),
});

export const { useSignUpMutation, useVerifEmailMutation } = userService;
