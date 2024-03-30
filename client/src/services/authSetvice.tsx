import {
  BaseQueryFn,
  FetchArgs,
  createApi,
  fetchBaseQuery,
} from "@reduxjs/toolkit/query/react";
import { API_URL } from "../constants";
import { RootState } from "../store/store";

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
      console.log(token);
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

export const { useLoginMutation } = authService;
