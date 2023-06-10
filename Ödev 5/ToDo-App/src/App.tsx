import { useState, ChangeEvent, useEffect } from "react";
import "./App.css";
import TodoDto from "./types/TodoDto";
import { v4 as uuidv4 } from "uuid";
import moment from 'moment';
import { Button, Card, Form, Icon, Input, Segment } from "semantic-ui-react";

function App() {
  const [task, setTask] = useState("");
  const [isSortedByDate, setIsSortedByDate] = useState(false);
  const [todoItem, setTodoArray] = useState<TodoDto[]>([]);

  useEffect(() => {
    // State güncellendiğinde yapılacak işlemler
  }, [todoItem]);

  const handleInputChange = (event: ChangeEvent<HTMLInputElement>) => {
    const value = event.target.value;
    setTask(value.trimStart());
  };

  const handleGenerateTask = (): void => {
    const todoDto = new TodoDto(uuidv4(), task, false, new Date());
    setTodoArray((prevArray) => [todoDto, ...prevArray]);
    setTask("");
  };

  const handleCrossTodo = (id: string) => {
    const updatedArray = todoItem.map((todo) =>
      todo.id === id ? { ...todo, isCompleted: !todo.isCompleted } : todo
    );
    setTodoArray(updatedArray);
  };

  const handleDeleteToDoItem = (id: string) => {
    const remainArray = todoItem.filter((todo) => todo.id !== id);
    setTodoArray(remainArray);
  };

  const handleSortByDateArray = (): void => {
    const sortedArray = [...todoItem].sort((a, b) => {
      const dateA = moment(a.createdDate);
      const dateB = moment(b.createdDate);
      return dateA.diff(dateB);
    });
    setTodoArray(sortedArray);
    //setTodoArray((prevArray) => [...prevArray].reverse());
     setIsSortedByDate(!isSortedByDate);
  };

  const getIconClassName = () => {
    if (isSortedByDate) {
      return "blue";
    } else {
      return "pink";
    }
  };

  return (
    <>
      <Segment basic className="card-align" style={{ padding: "100px" }}>
        <Card className="ui card custom-card">
          <Card.Content basic className="content">
            <Card.Header
              basic
              className={`center aligned ui huge font-custom ${getIconClassName()}`}
              style={{ margin: 0 }}
            >
              To-Do App
            </Card.Header>
            <Card.Description className="description" style={{ margin: 0 }}>
              <Segment basic style={{ margin: 0 }}>
                <Form.Field className="ui action input">
                  <Input
                    type="text"
                    maxLength={40}
                    value={task}
                    onChange={handleInputChange}
                  />
                  <Button
                    className={`blue font-custom ${getIconClassName()}`}
                    disabled={!task}
                    onClick={handleGenerateTask}
                    size="big"
                  >
                    Ekle
                  </Button>
                  <Button
                    className="clear-background"
                    onClick={handleSortByDateArray}
                  >
                    <Icon
                      size="big"
                      name={
                        isSortedByDate ? "angle double down" : "angle double up"
                      }
                      className={getIconClassName()}
                    />
                  </Button>
                </Form.Field>
              </Segment>
              <Segment
                basic
                className="font-custom scrollable"
                style={{ margin: 0 }}
              >
                {todoItem.map((todo) => (
                  <Segment
                    basic
                    className="todoItem-align"
                    size="big"
                    key={todo.id}
                    onDoubleClick={() => handleCrossTodo(todo.id)}
                  >
                    <Segment.Inline
                      className="changeDisplay"
                      style={{
                        textDecoration: todo.isCompleted
                          ? "line-through"
                          : "none",
                      }}
                    >
                      {todo.task}
                    </Segment.Inline>
                    <Icon
                      name="trash alternate"
                      key={todo.id}
                      onClick={() => handleDeleteToDoItem(todo.id)}
                    />
                  </Segment>
                ))}
              </Segment>
            </Card.Description>
          </Card.Content>
        </Card>
      </Segment>
      <Segment
        basic
        size="big"
        className="font-custom"
        style={{
          position: "fixed",
          left: 0,
          bottom: 0,
          width: "100%",
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          backgroundColor: "transparent",
          zIndex: 9999,
          color: "white",
          textShadow: "1px 1px 2px blue",
          
        }}
      >
        Created by Neslihan Çakırbaş
      </Segment>
    </>
  );
}

export default App;
