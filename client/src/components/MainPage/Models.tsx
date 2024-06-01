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

const Models = () => {
  const [getAuthKeyboards, { data : authKeyboards }] = useLazyGetDefaultKeyboardsQuery();

  const [getKeyboards, { data : baseKeyboards }] = useLazyGetAuthKeyboardsQuery();
  
  const [uniqueKeyboard ,setUniqueKeyboard] = useState<any[]>();
  useEffect(() => {
    const data = {
      page: 1,
      pageSize: 100,
    }
    getAuthKeyboards(data);
    getKeyboards(data); 
  }, [])

  useEffect(() => {
    if (authKeyboards && baseKeyboards)
      setUniqueKeyboard(Array.from(new Set(authKeyboards.concat(baseKeyboards))));
    if (baseKeyboards) setUniqueKeyboard(baseKeyboards);
    if (authKeyboards) setUniqueKeyboard(authKeyboards);
  }, [baseKeyboards, authKeyboards]);

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
        //style={{paddingLeft: "50px", paddingRight: "50px"}}
        pagination={{
          clickable: true,
        }}
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
        <SwiperSlide>
          <PreviewCard />
        </SwiperSlide>
        {uniqueKeyboard?.map((value : any) => {
          return (
            <SwiperSlide>
              <PreviewCard keyBoardId={value.id} title={value.title}/>
            </SwiperSlide>
          )})}
        {!authKeyboards && baseKeyboards && baseKeyboards?.map((value : any) => {
          return (
            <SwiperSlide>
              <PreviewCard keyBoardId={value.id} title={value.title}/>
            </SwiperSlide>
          )})}
       

      </Swiper>
    </Container>
  );
};

export default Models;
