import { BaseQueryFn, FetchArgs, coreModule, fetchBaseQuery, reactHooksModule, retry } from "@reduxjs/toolkit/query/react";
import { API_URL } from "../constants";
import { RootState } from '../store/store';
import { logOut, updateToken } from "../store/authSlice";

const refreshBaseQuery = fetchBaseQuery({
  baseUrl: API_URL,
  credentials: 'include',
  prepareHeaders: (headers) => {
    headers.set('Accept', '*/*');
    return headers;
  },
});

const baseQuery = fetchBaseQuery({
  baseUrl: API_URL,
  credentials: 'include',
  prepareHeaders: (headers, { getState }) => {
    const state = getState() as RootState; 
    const token = (getState() as RootState).authReducer.accessToken;
    if (token) {
      headers.set('Authorization', `Bearer ${token}`);
    }
    return headers;
  },
});

export const getBaseQuery: BaseQueryFn<string | FetchArgs, unknown, unknown> = async (args, api, extraOptions) => {
  let result = await baseQuery(args, api, extraOptions);
  const state = api.getState() as RootState; 
  const accessToken = state.authReducer.accessToken;

  if (result.error && result.error.status === 401 && accessToken) {
    const refreshResult = await refreshBaseQuery('users/refresh', api, extraOptions);
    if (refreshResult.data) {
      console.log("🚀 ~ Token refreshed successfully");
      // @ts-ignore
      api.dispatch(updateToken({ accessToken: refreshResult.data.accessToken }));

      result = await baseQuery(args, api, extraOptions);
    } else {
      console.log("🚀 ~ Token refresh error");
      api.dispatch(logOut());
    }
  }

  return result;
};