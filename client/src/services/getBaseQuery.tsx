import { BaseQueryFn, FetchArgs, coreModule, fetchBaseQuery, reactHooksModule, retry }  from "@reduxjs/toolkit/query/react";
import { API_URL } from "../constants";
import { RootState } from '../store/store'
import authSlice, { logOut, updateToken } from "../store/authSlice";

const refreshBaseQuery = fetchBaseQuery({
  baseUrl: API_URL,
  credentials: 'include',
  prepareHeaders: async (headers) => {

    headers.set('Accept',`*/*`);

    return headers;
  },
})

const baseQuery = fetchBaseQuery({
  baseUrl: API_URL,
  credentials: 'include',
  prepareHeaders: async (headers, api) => {

    let token = (api.getState() as RootState).authReducer.accessToken;
    token && headers.set('Authorization', `Bearer ${token}`);

    return headers;
  },
})

export const getBaseQuery: BaseQueryFn<string | FetchArgs, unknown, unknown>
  = async (args, api, extraOption) => {
    let result = await baseQuery(args, api, extraOption)

    if (result?.error && result.error.status === 401) {
        let tryRefreshToken = await refreshBaseQuery('users/refresh', api, extraOption); 
        console.log(tryRefreshToken) 
        // @ts-ignore
        if (tryRefreshToken?.data) {
          console.log("🚀 ~ = ~ Token refreshed successfully")
          // @ts-ignore
          api.dispatch(updateToken({ accessToken: tryRefreshToken.data.accessToken }))

          result = await baseQuery(args, api, extraOption)
          return result
        } else {
          console.log("🚀 ~ = ~ Token refreshed error")
          api.dispatch(logOut())
        }
    }
    return result
  }
