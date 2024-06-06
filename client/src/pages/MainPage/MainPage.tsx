import Header from "../../components/Header/Header";
import { Container } from "@mui/material";
import { useGetAuthKeyboardsQuery } from "../../services/keyboardService";

import Title from "../../components/MainPage/Title";
import About from "../../components/MainPage/About";
import Scheme from "../../components/MainPage/Scheme";
import Models from "../../components/MainPage/Models";
import ScrollButton from "../../components/MainPage/ScrollButton";
import Constructor from "../../components/MainPage/Constructor";
import Footer from "../../components/Footer/Footer";

const MainPage = () => {

  return (
    <>
      <Header />
      <Container maxWidth={false} disableGutters>
        <Title />
        <About />
        <Scheme />
        <Models />
        <Constructor />
        <ScrollButton/>
        <Footer />
      </Container>
    </>
  );
};

export default MainPage;
