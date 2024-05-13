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
        Мы предоставляем возможность собрать свою клавиатуру и послушать звук нажатия её клавиш в интерактивной 3D-среде.
      </Typography>
      <Typography fontSize={20}>
        На рынке клавиатур существует множество переключателей разного типа и звучания. 
      </Typography>
      <Typography fontSize={20} sx={{ mb: "50px" }}>
        <Box component="span" fontWeight="bold">
          Наша платформа поможет Вам выбрать тот переключатель, который будет по душе!
        </Box>
      </Typography>
    </Container>
  )
};

export default About;
