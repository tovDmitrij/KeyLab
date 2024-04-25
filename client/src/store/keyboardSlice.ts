import { createSlice, PayloadAction } from "@reduxjs/toolkit";

/**
 * Интерфейс для представления готовой клавиатуры
 */
type TKeyboardState = {
  switchTypeID?: string;
  boxTypeID?: string;
  title?: string;

};

const initialState: TKeyboardState = {
  switchTypeID: undefined,
  boxTypeID: undefined,
  title: undefined,
};

export const profileSlice = createSlice({
  name: "profile",
  initialState: initialState,
  reducers: {
    setTitle: (state, action: PayloadAction<string | undefined>) => {
      state.title = action.payload;
    },

    setSwitchTypeID: (state, action: PayloadAction<string | undefined>) => {
      state.switchTypeID = action.payload;
    },

    setBoxTypeID: (state, action: PayloadAction<string | undefined>) => {
      state.boxTypeID = action.payload;
    },
  },
});

export const { setTitle, setSwitchTypeID, setBoxTypeID } = profileSlice.actions;
export default profileSlice.reducer;
