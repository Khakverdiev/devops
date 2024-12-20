import React, { useState, useEffect } from 'react';

const TaskList = () => {
    const [tasks, setTasks] = useState([]);
    const [selectedTask, setSelectedTask] = useState(null); // Для отображения задачи по ID
    const [updateMode, setUpdateMode] = useState(false); // Для переключения между режимами обновления

    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [isCompleted, setIsCompleted] = useState(false);

    // Загрузка всех задач
    useEffect(() => {
        fetchTasks();
    }, []);

    const fetchTasks = () => {
        fetch('http://localhost:8080/api/task')
            .then((res) => res.json())
            .then((data) => setTasks(data))
            .catch((err) => console.error(err));
    };

    // Удаление задачи
    const deleteTask = (id) => {
        fetch(`http://localhost:8080/api/task/${id}`, { method: 'DELETE' })
            .then(() => fetchTasks()) // Обновляем список после удаления
            .catch((err) => console.error(err));
    };

    // Получение задачи по ID
    const getTaskById = (id) => {
        fetch(`http://localhost:8080/api/task/${id}`)
            .then((res) => res.json())
            .then((task) => setSelectedTask(task))
            .catch((err) => console.error(err));
    };

    return (
        <div style={{ maxWidth: '600px', margin: '20px auto', padding: '20px' }}>
            <h1 style={{ textAlign: 'center', color: '#333', marginBottom: '20px' }}>Task List</h1>

            <ul
                style={{
                    listStyleType: 'none',
                    padding: 0,
                    border: '1px solid #ccc',
                    borderRadius: '8px',
                    boxShadow: '0 4px 6px rgba(0, 0, 0, 0.1)',
                }}
            >
                {tasks.map((task) => (
                    <li
                        key={task.id}
                        style={{
                            padding: '15px',
                            borderBottom: '1px solid #eee',
                            backgroundColor: '#f9f9f9',
                            display: 'flex',
                            justifyContent: 'space-between',
                            alignItems: 'center',
                        }}
                    >
                        <div>
                            <strong>{task.title}</strong> <br />
                            <small>{task.description}</small>
                            <br />
                            <span
                                style={{
                                    fontSize: '12px',
                                    color: task.isCompleted ? '#4CAF50' : '#FF5722',
                                    fontWeight: 'bold',
                                }}
                            >
                                {task.isCompleted ? 'Completed' : 'Pending'}
                            </span>
                        </div>
                        <div>
                            <button
                                onClick={() => deleteTask(task.id)}
                                style={{
                                    margin: '0 5px',
                                    padding: '5px 10px',
                                    backgroundColor: '#FF5722',
                                    color: '#fff',
                                    border: 'none',
                                    borderRadius: '4px',
                                    cursor: 'pointer',
                                }}
                            >
                                Delete
                            </button>
                            <button
                                onClick={() => getTaskById(task.id)}
                                style={{
                                    margin: '0 5px',
                                    padding: '5px 10px',
                                    backgroundColor: '#007BFF',
                                    color: '#fff',
                                    border: 'none',
                                    borderRadius: '4px',
                                    cursor: 'pointer',
                                }}
                            >
                                View
                            </button>
                        </div>
                    </li>
                ))}
                {tasks.length === 0 && (
                    <li
                        style={{
                            padding: '15px',
                            textAlign: 'center',
                            color: '#999',
                            fontStyle: 'italic',
                        }}
                    >
                        No tasks found
                    </li>
                )}
            </ul>

            {selectedTask && (
                <div style={{ marginTop: '20px' }}>
                    <h2>Selected Task</h2>
                    <p><strong>Title:</strong> {selectedTask.title}</p>
                    <p><strong>Description:</strong> {selectedTask.description}</p>
                    <p><strong>Status:</strong> {selectedTask.isCompleted ? 'Completed' : 'Pending'}</p>
                </div>
            )}
        </div>
    );
};

export default TaskList;
