import authSlice from './authSlice.ts';
import { authService, userService } from "../services/userService";
import { keyboardsService } from '../services/keyboardService.tsx';
import profileSlice from './profileSlice.ts';
import { switchesService } from '../services/switchesService.tsx';
import { boxesService } from '../services/boxesService.tsx';
import { keycapsService } from '../services/keycapsService.tsx';
import { kitsService } from '../services/kitsService.tsx';
import { statsService } from '../services/statsService.tsx';
import statsSlice from './statsSlice.ts';
import keyboardSlice from './keyboardSlice.ts';

const reducers = {
  authReducer: authSlice,
  profileReducer: profileSlice,
  statsReducer: statsSlice,
  keyboardReduer: keyboardSlice,
  
  [authService.reducerPath]: authService.reducer,
  [userService.reducerPath]: userService.reducer,
  [keyboardsService.reducerPath]: keyboardsService.reducer,
  [switchesService.reducerPath]: switchesService.reducer,
  [boxesService.reducerPath]:boxesService.reducer,
  [keycapsService.reducerPath]: keycapsService.reducer,
  [kitsService.reducerPath]: kitsService.reducer,
  [statsService.reducerPath]: statsService.reducer,
};

export default reducers;