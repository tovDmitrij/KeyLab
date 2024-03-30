import { combineReducers, configureStore } from "@reduxjs/toolkit";
import { userService } from "../services/userService";
import reducers from './index';
import { keyboardsService } from "../services/keyboardService";
import { authService } from "../services/authSetvice";

const rootReducer = combineReducers(reducers);

export const store = configureStore({
  reducer: rootReducer,
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(
      userService.middleware,
      authService.middleware,
      keyboardsService.middleware
    )
});

export type RootState = ReturnType<typeof rootReducer>;
export type AppDispatch = typeof store.dispatch;