import { createSlice, PayloadAction } from "@reduxjs/toolkit";

/**
 * Интерфейс для представления состояния аутентификации.
 */
type TAuthState = {
  /**
   * Флаг, указывающий, прошла ли аутентификация.
   */
  isAuth: boolean;

  /**
   * JWT токен, связанный с аутентифицированным пользователем.
   */
  accessToken?: string;
};

const initialState: TAuthState = {
  isAuth: !!localStorage.getItem("token"),
  accessToken: localStorage.getItem("token") || undefined,
};

export const authSlice = createSlice({
  name: "auth",
  initialState: initialState,
  reducers: {
    /**
     * Action creator для входа пользователя в систему.
     */
    singnIn: (state, action) => {
      const accesToken = action.payload.accessToken;
      state.accessToken = accesToken;  
      localStorage.setItem("token", accesToken);
      state.isAuth = true;
    },

    /**
     * Action creator для выхода пользователя из системы.
     */
    logOut: (state) => {
      //state.user = null;
      localStorage.removeItem("admin");
      localStorage.removeItem("token");
      state.isAuth = false;
      state.accessToken = undefined;
    },

    updateToken: (state, action) => {
      const accesToken = action.payload.accessToken;
      state.accessToken = accesToken;
      localStorage.setItem("token", accesToken);
    },
  },
});

export const { updateToken, singnIn, logOut } = authSlice.actions;
export default authSlice.reducer;
