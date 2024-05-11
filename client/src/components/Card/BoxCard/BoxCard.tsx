import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Typography from "@mui/material/Typography";
import { CardActionArea } from "@mui/material";
import IconPlus from "../../Icons/IconPlus/IconPlus";

import classes from "./BoxCard.module.scss";
import { useNavigate } from "react-router-dom";
import { useAppSelector } from "../../../store/redux";
import Preview from "../../List/SwitchesList/Preview";

const BoxCard = () => {
  const navigate = useNavigate();
  const { boxTitle, boxID } = useAppSelector(
    (state) => state.keyboardReduer
  );


  return (
    <Card className={classes.card} sx={{ borderRadius: 10 }}>
      <CardActionArea onClick={() => navigate("/constrBoxes")}>
      <CardContent className={classes.card_content}>
          {!boxTitle ? (
            <>
              <Typography sx={{ mt: 1, fontSize: 20, color: "#AEAEAE" }}>
                Выберите размер клавиатуры
              </Typography>
              <IconPlus sx={{ fontSize: 50 }} />
            </>
          ) : (
            <>
              <Preview id={boxID} type="box" width={300} height={165}/>
              <Typography sx={{ mt: 1, fontSize: 20, color: "#AEAEAE" }}>
                {boxTitle}
              </Typography>
            </>
          )}
        </CardContent>
      </CardActionArea>
    </Card>
  );
};

export default BoxCard;
