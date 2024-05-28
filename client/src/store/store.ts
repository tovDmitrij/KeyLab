import { combineReducers, configureStore } from "@reduxjs/toolkit";
import { userService } from "../services/userService";
import reducers from './index';
import { keyboardsService } from "../services/keyboardService";
import { authService } from "../services/authService";
import { switchesService } from "../services/switchesService";
import { boxesService } from "../services/boxesService";
import { kitsService } from "../services/kitsService";
import { keycapsService } from "../services/keycapsService";
import { statsService } from "../services/statsService";

const rootReducer = combineReducers(reducers);

export const store = configureStore({
  reducer: rootReducer,
  middleware: (getDefaultMiddleware) =>
    
    getDefaultMiddleware({serializableCheck: false}).concat(
      userService.middleware,
      authService.middleware,
      keyboardsService.middleware,
      switchesService.middleware,
      boxesService.middleware,
      kitsService.middleware,
      keycapsService.middleware,
      statsService.middleware,
    )
});

export type RootState = ReturnType<typeof rootReducer>;
export type AppDispatch = typeof store.dispatch;