import { createSlice, PayloadAction } from "@reduxjs/toolkit";

/**
 * Интерфейс для представления готовой клавиатуры
 */
type TKeyboardState = {
  typeSizeBox?: string;
  typeSizeKit?: string;
  switchTitle?: string;
  switchTypeID?: string;
  boxTypeId?: string;
  boxID?: string;
  boxTitle?: string;
  title: string;
  kitTitle?: string;
  kitID?: string;
};

const initialState: TKeyboardState = {
  switchTitle: undefined,
  switchTypeID: undefined,
  boxID: undefined,
  title: "Безымянный",
};

export const keyboardSlice = createSlice({
  name: "keyboard",
  initialState: initialState,
  reducers: {
    setTypeSizeBox: (state, action: PayloadAction<string | undefined>) => {
      state.typeSizeBox = action.payload;
    },

    setTypeSizeKit: (state, action: PayloadAction<string | undefined >) => {
      state.typeSizeKit = action.payload;
    },

    setTitle: (state, action: PayloadAction<string >) => {
      state.title = action.payload;
    },

    setBoxTypeId: (state, action: PayloadAction<string | undefined>) => {
      state.boxTypeId = action.payload;
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

    setKitTitle: (state, action: PayloadAction<string | undefined>) => {
      state.kitTitle = action.payload;
    },

    setKitID: (state, action: PayloadAction<string | undefined>) => {
      state.kitID = action.payload;
    },

    resetKeyBoardState: (state) => {
      state.typeSizeBox = undefined;
      state.typeSizeKit = undefined;
      state.switchTitle = undefined;
      state.switchTypeID= undefined;
      state.boxTypeId= undefined;
      state.boxID= undefined;
      state.boxTitle= undefined;
      state.title= "Безымянный";
      state.kitTitle= undefined;
      state.kitID= undefined;
    }
  },
});

export const {
  setTypeSizeKit,
  setTypeSizeBox,
  resetKeyBoardState,
  setBoxTypeId,
  setTitle,
  setSwitchTypeID,
  setBoxID,
  setSwitchTitle,
  setBoxTitle,
  setKitID,
  setKitTitle,
} = keyboardSlice.actions;
export default keyboardSlice.reducer;
