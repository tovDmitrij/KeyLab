import { FC, useEffect, useRef, useState } from "react";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemText from "@mui/material/ListItemText";
import { Button, Container, Typography } from "@mui/material";
import { PopoverPicker } from "../../ColorPicker/PopoverPicker";
import { RgbaColor } from "react-colorful";

type TKeycaps = {
  /**
   * id кита
   */
  id?: string;
  /**
   * название кита
   */
  title?: string;

  /**
   * дата создания бокса
   */
  creationDate?: string;
};

type Props = {
  keykaps?: TKeycaps[];
  handleChooseKey: (data: string) => void;
  handleChooseColor: (data: any) => void;
};

const KeycapsList: FC<Props> = ({ keykaps, handleChooseKey, handleChooseColor }) => {
  const [colors, setColors] = useState<{ [key: string]: RgbaColor }>({});

  const onChange = (id: string, newColor: RgbaColor) => {
    if (!id) return; 
    setColors(prevColors => ({
      ...prevColors,
      [id]: newColor,
    }));
    handleChooseColor(newColor);
  };

  const onClick = (value: TKeycaps) => {
    if (!value.id) return;
    handleChooseKey(value.id);
  };

  const popover = useRef(null);

  return (
    <Container
      disableGutters
      sx={{
        width: '100%',
        textAlign: 'center',
        bgcolor: '#2A2A2A',
        height: '100vh',
        overflow: 'hidden',
      }}
    >
      <List
        sx={{
          mt: '66px',
          height: '76%',
          position: 'relative',
          overflowY: 'auto',
        }}
      >
        {keykaps?.map((value) => {
          const labelId = `checkbox-list-label-${value.id}`;

          return (
            <ListItem
              sx={{ textAlign: 'center', minWidth: '100' }}
              key={value.id}
              disablePadding
            >
              <ListItemButton
                role={undefined}
                onClick={() => onClick(value)}
                dense
              >
                <ListItemText
                  sx={{
                    color: 'white',
                    m: '5px',
                  }}
                  id={labelId}
                  primary={`${value.title}`}
                />

                {/* {value.id && <PopoverPicker
                  color={colors[value.id] || { r: 200, g: 150, b: 35, a: 0.5 }}
                  onChange={(newColor: RgbaColor) => onChange(value.id || '', newColor)}
                />} */}
              </ListItemButton>                
              {value.id && <PopoverPicker
                  color={colors[value.id] || { r: 200, g: 150, b: 35, a: 0.5 }}
                  onChange={(newColor: RgbaColor) => onChange(value.id || '', newColor)}
                />}
            </ListItem>
          );
        })}
      </List>
      <Container>
        <Button
          sx={{
            m: '15px',
            width: '90%',
            borderRadius: '30px',
          }}
          variant="contained"
        >
          назад
        </Button>
      </Container>
    </Container>
  );
};

export default KeycapsList;