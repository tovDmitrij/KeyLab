import authSlice from './authSlice.ts';
import { authService, userService } from "../services/userService";
import { keyboardsService } from '../services/keyboardService.tsx';
import profileSlice from './profileSlice.ts';
import { switchesService } from '../services/switchesService.tsx';
import { boxesService } from '../services/boxesService.tsx';

const reducers = {
  authReducer: authSlice,
  profileReducer: profileSlice,
  
  [authService.reducerPath]: authService.reducer,
  [userService.reducerPath]: userService.reducer,
  [keyboardsService.reducerPath]: keyboardsService.reducer,
  [switchesService.reducerPath]: switchesService.reducer,
  [boxesService.reducerPath]:boxesService.reducer,

};

export default reducers;