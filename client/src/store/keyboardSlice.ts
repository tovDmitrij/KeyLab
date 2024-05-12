import { createSlice, PayloadAction } from "@reduxjs/toolkit";

/**
 * Интерфейс для представления готовой клавиатуры
 */
type TKeyboardState = {
  switchTitle?: string;
  switchTypeID?: string;
  boxID?: string;
  boxTitle?: string;
  title?: string;
};

const initialState: TKeyboardState = {
  switchTitle: undefined,
  switchTypeID: undefined,
  boxID: undefined,
  title: undefined,
};

export const keyboardSlice = createSlice({
  name: "keyboard",
  initialState: initialState,
  reducers: {
    setTitle: (state, action: PayloadAction<string | undefined>) => {
      state.title = action.payload;
    },

    setSwitchTitle: (state, action: PayloadAction<string | undefined>) => {
      state.switchTitle = action.payload;
    },

    setSwitchTypeID: (state, action: PayloadAction<string | undefined>) => {
      state.switchTypeID = action.payload;
    },

    setBoxTitle: (state, action: PayloadAction<string | undefined>) => {
      state.boxTitle = action.payload;
    },

    setBoxID: (state, action: PayloadAction<string | undefined>) => {
      state.boxID = action.payload;
    },
  },
});

export const { setTitle, setSwitchTypeID, setBoxID, setSwitchTitle, setBoxTitle } = keyboardSlice.actions;
export default keyboardSlice.reducer;
