import { Route, Routes } from "react-router-dom";
import Login from "../pages/Login/Login";
import Register from "../pages/Register/Register";
import MainPage from "../pages/MainPage/MainPage";
import Constructors from "../pages/Constructors/ConstructorsMain";
import ConstructorsSwitches from "../pages/ConstructorsSwitches/ConstructorsSwitches";
import ConstrucrotBoxes from "../pages/ConstructorsBoxes/ConstructorsBoxes";
import ConstructorKeys from "../pages/ConstructorsKeys/ConstructorsKeys";
import Stats from "../pages/Stats/Stats";
import { useAppSelector } from "../store/redux";

const AppRouter = () => {
  const { isAdmin } = useAppSelector(
    (state) => state.profileReducer
  );

  return (
    <Routes>
      <Route path="/constrKeys" element={<ConstructorKeys/>} />
      <Route path="/constrBoxes" element={<ConstrucrotBoxes/>} />
      <Route path="/constrSwitch" element={<ConstructorsSwitches />} />
      <Route path="/constructors" element={<Constructors />} />
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />
      <Route path="/stats" element={<Stats />} />
      <Route path="/" element={<MainPage />} />
    </Routes>
  );
};

export default AppRouter;
