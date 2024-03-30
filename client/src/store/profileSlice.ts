import { createSlice, PayloadAction } from "@reduxjs/toolkit";

/**
 * Интерфейс для представления пользовательского профиея
 */
type TUserState = {
  nickName?: string;
  email?: string;
};

const initialState: TUserState = {
  nickName: undefined,
  email: undefined,
};

export const profileSlice = createSlice({
  name: "profile",
  initialState: initialState,
  reducers: {
    setNickName: (state, action: PayloadAction<string | undefined>) => {
      state.nickName = action.payload;
    },

    setEmail: (state, action: PayloadAction<string | undefined>) => {
      state.email = action.payload;
    },
  },
});

export const { setNickName, setEmail } = profileSlice.actions;
export default profileSlice.reducer;
