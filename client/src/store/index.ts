import authSlice from './authSlice.ts';
import { authService, userService } from "../services/userService";
import { keyboardsService } from '../services/keyboardService.tsx';
import profileSlice from './profileSlice.ts';

const reducers = {
  authReducer: authSlice,
  profileReducer: profileSlice,
  
  [authService.reducerPath]: authService.reducer,
  [userService.reducerPath]: userService.reducer,
  [keyboardsService.reducerPath]: keyboardsService.reducer,

};

export default reducers;