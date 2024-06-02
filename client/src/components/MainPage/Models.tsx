import { useNavigate } from "react-router-dom";
import { Box, Container, Grid, Typography } from "@mui/material";
import PreviewCard from "../Card/PreviewCard/PreviewCard";
import { Swiper, SwiperSlide } from "swiper/react";
import "swiper/css";
import "swiper/css/pagination";
import 'swiper/css/navigation';
import { Pagination, Navigation } from 'swiper/modules';
import { useGetAuthKeyboardsQuery, useGetDefaultKeyboardsQuery, useLazyGetAuthKeyboardsQuery, useLazyGetDefaultKeyboardsQuery } from "../../services/keyboardService";
import { useEffect, useState } from "react";

import classes from "./models.module.scss";

const Models = () => {
  const [getAuthKeyboards, { data: authKeyboards }] = useLazyGetAuthKeyboardsQuery();
  const [getBaseKeyboards, { data: baseKeyboards }] = useLazyGetDefaultKeyboardsQuery();
  
  const [uniqueKeyboard, setUniqueKeyboard] = useState<any[]>([]);

  useEffect(() => {
    const data = {
      page: 1,
      pageSize: 100,
    };
    getAuthKeyboards(data);
    getBaseKeyboards(data); 
  }, [getAuthKeyboards, getBaseKeyboards]);

  useEffect(() => {
    if (authKeyboards || baseKeyboards) {
      const mergedKeyboards = [...(authKeyboards || []), ...(baseKeyboards || [])];
      const uniqueKeyboardsMap = new Map();
  
      mergedKeyboards.forEach(keyboard => {
        uniqueKeyboardsMap.set(keyboard.id, keyboard);
      });
  
      setUniqueKeyboard(Array.from(uniqueKeyboardsMap.values()));
    }
  }, [authKeyboards, baseKeyboards]);

  return (
    <Container
      id="models"
      maxWidth={false}
      sx={{ width: "86%" }}
      className={classes.button}
      disableGutters
    >
      <Typography fontSize={32} sx={{ textAlign: "center", pt: "50px" }}>
        3D - модели
      </Typography>

      <Swiper
        pagination={{
          clickable: true,
        }}
        // style={{width: "100%",}}        
        breakpoints={{
          '@0.00': {
            slidesPerView: 1,
            spaceBetween: 10,
          },
          '@0.8': {
            slidesPerView: 2,
            spaceBetween: 10,
          },
          '@1.2': {
            slidesPerView: 3,
            spaceBetween: 10,
          },
          '@1.50': {
            slidesPerView: 4,
            spaceBetween: 10,
          },
          '@2.00': {
            slidesPerView: 5,
            spaceBetween: 10,
          },
        }}
        navigation={true}
        modules={[Pagination, Navigation]}
      > 
        <SwiperSlide key="add">
          <PreviewCard />
        </SwiperSlide>
        {uniqueKeyboard?.map((value: any) => (
          <SwiperSlide key={value.id}>
            <PreviewCard keyBoardId={value.id} title={value.title} />
          </SwiperSlide>
        ))}
      </Swiper>
    </Container>
  );
};

export default Models;