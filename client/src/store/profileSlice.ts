import { createSlice, PayloadAction } from "@reduxjs/toolkit";

/**
 * Интерфейс для представления пользовательского профиея
 */
type TUserState = {
  nickName?: string;
  email?: string;
  isAdmin?: boolean;
};

const initialState: TUserState = {
  nickName: undefined,
  email: undefined,
  isAdmin: !!localStorage.getItem("isAdmin")
}; 

export const profileSlice = createSlice({
  name: "profile",
  initialState: initialState,
  reducers: {
    setNickName: (state, action: PayloadAction<string | undefined>) => {
      const nickName = action.payload;
      state.nickName = nickName;
      localStorage.setItem("nickName", nickName || '');
    },

    setEmail: (state, action: PayloadAction<string | undefined>) => {
      state.email = action.payload;
    },

    setIsAdmin: (state, action: PayloadAction<boolean>) => {
      const isAdmin = action.payload; 
      state.isAdmin = isAdmin;
      localStorage.setItem("isAdmin", JSON.stringify(isAdmin));
    },

    resetState: (state) => {
      state.isAdmin = false;
      state.nickName = undefined;
      localStorage.removeItem("isAdmin");
      localStorage.removeItem("nickName");
    }
  },
});

export const { setNickName, setEmail, setIsAdmin, resetState } = profileSlice.actions;
export default profileSlice.reducer;
