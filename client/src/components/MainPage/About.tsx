import { Box, Button, Container, Typography } from "@mui/material";

const About = () => {
  return (
    <Container
      id = "about"
      maxWidth={false}
      sx={{
        alignItems: "center",
        textAlign: "center",
      }}
      disableGutters
    >
      <Typography fontSize={32} sx={{ mt: "50px" }}>
        О нас
      </Typography>
      <Typography fontSize={20} sx={{ mt: "20px" }}>
        Мы предоставляем возможность собрать свою клавиатуру и послушать ее
        звучание в интерактивной 3D среде
      </Typography>
      <Typography fontSize={20}>
        В мире клавиатур существует множество переключателей разного типа и
        разного звучания
      </Typography>
      <Typography fontSize={20} sx={{ mb: "50px" }}>
        <Box component="span" fontWeight="bold">
          Наша разработка поможет вам выбрать тот переключатель, который будет
          вам по душе!
        </Box>
      </Typography>
    </Container>
  )
};

export default About;
