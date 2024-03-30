import { BrowserRouter, Route, Routes } from "react-router-dom";
import Login from "../pages/Login/Login";
import Register from "../pages/Register/Register";
import MainPage from "../pages/MainPage/MainPage";

const AppRouter = () => {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />
      <Route path="/mainpage" element={<MainPage />} />
    </Routes>
  );
};

export default AppRouter;
