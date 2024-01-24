import { configureStore } from "@reduxjs/toolkit";
import { userService } from "../services/userService";

export const store = configureStore({
  reducer: {
    [userService.reducerPath]: userService.reducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(userService.middleware),
});
