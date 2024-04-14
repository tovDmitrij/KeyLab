import {
  BaseQueryFn,
  FetchArgs,
  createApi,
  fetchBaseQuery,
} from "@reduxjs/toolkit/query/react";
import { API_URL } from "../constants";
import { RootState } from "../store/store";
import { getBaseQuery } from "./getBaseQuery";

type TUser = {
  /**
   * Пароль пользователя.
   */
  password: string | null;
  /**
   * Email
   */
  email: string | null;
};

export const getBaseQueryAuth = (
  urlPrefix: string,
  URL: string | undefined = undefined
) =>
  fetchBaseQuery({
    baseUrl: (URL || API_URL) + urlPrefix,
    credentials: "include",
    prepareHeaders: (headers, { getState }) => {
      const token = (getState() as RootState).authReducer.accessToken;
      token && headers.set("Authorization", `Bearer ${token}`);
      return headers;
    },
  }) as BaseQueryFn<string | FetchArgs, unknown, unknown>;

export const authService = createApi({
  reducerPath: "authService",
  baseQuery: getBaseQueryAuth("users/"),
  endpoints: (builder) => ({
    login: builder.mutation<any, TUser>({
      query: (body) => ({
        url: "signIn",
        method: "POST",
        body: body,
      }),
    }),
  }),
});

export const userService = createApi({
  reducerPath: "userService",
  baseQuery: getBaseQuery,
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
    getNickName: builder.query<any, void>({
      query: () => ({
        url: "profiles/nickname",
        method: "GET",
        responseHandler: (response) => response.text(),
      }),
    }),
  }),
});

export const {
  useSignUpMutation,
  useVerifEmailMutation,
  useGetNickNameQuery,
  useLazyGetNickNameQuery,
} = userService;
