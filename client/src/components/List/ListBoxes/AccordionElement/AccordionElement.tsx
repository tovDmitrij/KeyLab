import { FC, useEffect } from "react";

import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Grid,
  List,
  ListItem,
  ListItemButton,
  ListItemText,
  Typography,
} from "@mui/material";
import ArrowDropDownIcon from "@mui/icons-material/ArrowDropDown";
import {
  useGetAuthBoxesQuery,
  useGetBoxesQuery,
  useGetDefaultBoxesQuery,
  useLazyGetAuthBoxesQuery,
  useLazyGetDefaultBoxesQuery,
} from "../../../../services/boxesService";

type TBoxes = {
  /**
   * id бокса
   */
  id?: string;
  /**
   * название бокса
   */
  title?: string;
};

type props = {
  boxTypeId: string;
  name: string;
  handleChoose: (data: TBoxes) => void;
  handleNew: (idType: string, idBaseBox: string) => void;
};

const AccordionElement: FC<props> = ({
  boxTypeId,
  name,
  handleChoose,
  handleNew,
}) => {
  const [getAuthBoxes, { data: boxes }] = useLazyGetAuthBoxesQuery();

  const [getBaseBoxes, { data: boxesBase }]= useLazyGetDefaultBoxesQuery();

  const onClick = (value: TBoxes) => {
    if (!value.id) return;
    handleChoose(value);
    
  };

  const onClickNew = () => {
    if (!boxesBase || !boxesBase[0]?.id) return;
    handleNew(boxTypeId, boxesBase[0]?.id as string);
  };

  useEffect(() => {
    const data = {
      page: 1,
      pageSize: 100,
      typeID: boxTypeId,
    }
    getAuthBoxes(data);
    getBaseBoxes(data);
  }, [])

  return (
    <Accordion>
      <AccordionSummary
        sx={{
          bgcolor: "#2A2A2A",
          color: "#c1c0c0",
          borderTop: "1px solid grey",
          borderBottom: "1px solid grey",
          margin: "0 0 -1px 0px",
        }}
        expandIcon={
          <ArrowDropDownIcon sx={{ color: "#AEAEAE", fontSize: 40 }} />
        }
      >
        <Typography
          sx={{
            textAlign: "center",
            margin: "15px",
          }}
        >
          {name}
        </Typography>
      </AccordionSummary>
      <AccordionDetails
        sx={{
          p: 0,
          bgcolor: "#2A2A2A",
          color: "#FFFFFF",
        }}
      >
        <Grid container direction="column">
          <List disablePadding>
            {boxesBase &&
              boxes &&
              boxesBase.concat(boxes).map((value) => {
                const labelId = `checkbox-list-label-${value}`;

                return (
                  <>
                    <ListItem
                      sx={{ minWidth: "100" }}
                      key={value.id}
                      disablePadding
                    >
                      <ListItemButton
                        sx={{
                          textAlign: "center",
                          margin: "8px",
                          borderTop: "1px solid grey",
                          borderBottom: "1px solid grey",
                        }}
                        role={undefined}
                        onClick={() => onClick(value)}
                        dense
                      >
                        <ListItemText
                          sx={{
                            textAlign: "center",
                            color: "#c1c0c0",
                            m: "5px",
                          }}
                          id={labelId}
                          primary={`${value.title}`}
                        />
                      </ListItemButton>
                    </ListItem>
                  </>
                );
              })}
            <ListItem sx={{ minWidth: "100" }} disablePadding>
              <ListItemButton
                sx={{
                  textAlign: "center",
                  margin: "8px",
                  borderTop: "1px solid grey",
                  borderBottom: "1px solid grey",
                }}
                role={undefined}
                onClick={() => onClickNew()}
                dense
              >
                <ListItemText
                  sx={{
                    textAlign: "center",
                    color: "#c1c0c0",
                    m: "5px",
                  }}
                  primary={`Создать`}
                />
              </ListItemButton>
            </ListItem>
          </List>
        </Grid>
      </AccordionDetails>
    </Accordion>
  );
};

export default AccordionElement;
