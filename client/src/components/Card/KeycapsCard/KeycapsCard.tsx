import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Typography from "@mui/material/Typography";
import { CardActionArea } from "@mui/material";
import IconPlus from "../../Icons/IconPlus/IconPlus";

import classes from "./KeycapsCard.module.scss";
import { useNavigate } from "react-router-dom";

const KeycapsCard = () => {
  const navigate = useNavigate();

  return (
    <Card className={classes.card} sx={{ borderRadius: 10 }}>
      <CardActionArea onClick={() => navigate("/constrKeys")}>
        <CardContent className={classes.card_content}>
          <Typography sx={{ mt: 1, fontSize: 20, color: "#AEAEAE" }}>
            Выберите кнопки
          </Typography>
          <IconPlus sx={{ fontSize: 50 }} />{" "}
        </CardContent>
      </CardActionArea>
    </Card>
  );
};

export default KeycapsCard;
