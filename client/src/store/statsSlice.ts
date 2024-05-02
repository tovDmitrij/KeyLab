import { createSlice, PayloadAction } from "@reduxjs/toolkit";

type TStatsState = {
  leftDate?: string;
  rightDate?: string;
  intervalId?: string;
  activityIDs?: string[];
};

const initialState: TStatsState = {
  leftDate: undefined,
  rightDate: undefined,
  intervalId: undefined,
  activityIDs:undefined,
};

export const statsSlice = createSlice({
  name: "stats",
  initialState: initialState,
  reducers: {

    setLeftDate: (state, action) => {
      state.leftDate = action.payload;
    },

    setRightDate: (state, action) => {
      state.rightDate = action.payload;
    },

    setIntervalId: (state, action) => {
      state.intervalId = action.payload;
    },

    setActivityIDs: (state, action) => {
      state.activityIDs = action.payload;
    },
  },
});

export const { setActivityIDs, setIntervalId, setLeftDate, setRightDate  } = statsSlice.actions;
export default statsSlice.reducer;
