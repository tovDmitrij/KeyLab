import { useNavigate } from "react-router-dom";

import { Box, Container, Grid, Typography } from "@mui/material";
import PreviewCard from "../Card/PreviewCard/PreviewCard";
import { Swiper, SwiperSlide } from "swiper/react";

import "swiper/css";
import "swiper/css/pagination";
import 'swiper/css/navigation';
import { Pagination, Navigation } from 'swiper/modules';

const Models = () => {
  return (
    <Container
      id="models"
      maxWidth={false}
      sx={{ width: "86%" }}
      disableGutters
    >
      <Typography fontSize={32} sx={{ textAlign: "center", pt: "50px" }}>
        3D - модели
      </Typography>

      <Swiper
        style={{paddingLeft: "50px", paddingRight: "50px"}}
        slidesPerView={5}
        spaceBetween={30}
        pagination={{
          clickable: true,
        }}
        navigation={true}
        modules={[Pagination, Navigation]}
      >
        <SwiperSlide>
          <PreviewCard />
        </SwiperSlide>
        <SwiperSlide>
          <PreviewCard />
        </SwiperSlide>
        <SwiperSlide>
          <PreviewCard />
        </SwiperSlide>
        <SwiperSlide>
          <PreviewCard />
        </SwiperSlide>
        <SwiperSlide>
          <PreviewCard />
        </SwiperSlide>
        <SwiperSlide>
          <PreviewCard />
        </SwiperSlide>
        <SwiperSlide>
          <PreviewCard />
        </SwiperSlide>
        <SwiperSlide>
          <PreviewCard />
        </SwiperSlide>
        <SwiperSlide>
          <PreviewCard />
        </SwiperSlide>
      </Swiper>
    </Container>
  );
};

export default Models;
