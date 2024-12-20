import React, {useState} from 'react'

const AddTask = () => {
    const [title, setTitle] = useState('')
    const [description, setDescription] = useState('')

    const handleSubmit = (e) => {
        e.preventDefault();
        fetch('http://localhost:8080/api/task', {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({title, description, isComplated: false}),
        }).then(() => {
            setTitle('');
            setDescription('');
        });
    };

    return (
        <form
            onSubmit={handleSubmit}
            style={{
                maxWidth: '400px',
                margin: '20px auto',
                padding: '20px',
                border: '1px solid #ccc',
                borderRadius: '8px',
                boxShadow: '0 4px 6px rgba(0, 0, 0, 0.1)'
            }}
        >
            <h2 style={{ textAlign: 'center', marginBottom: '20px', color: '#333' }}>Add a New Task</h2>
            <input
                type="text"
                value={title}
                onChange={(e) => setTitle(e.target.value)}
                placeholder="Task Title"
                style={{
                    width: '100%',
                    padding: '10px',
                    marginBottom: '10px',
                    border: '1px solid #ccc',
                    borderRadius: '4px',
                    fontSize: '16px'
                }}
            />
            <textarea
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                placeholder="Task Description"
                style={{
                    width: '100%',
                    padding: '10px',
                    marginBottom: '10px',
                    border: '1px solid #ccc',
                    borderRadius: '4px',
                    fontSize: '16px',
                    resize: 'none',
                    height: '100px'
                }}
            />
            <button
                type="submit"
                style={{
                    width: '100%',
                    padding: '10px',
                    backgroundColor: '#4CAF50',
                    color: '#fff',
                    border: 'none',
                    borderRadius: '4px',
                    fontSize: '16px',
                    cursor: 'pointer'
                }}
            >
                Add Task
            </button>
        </form>
    );
}

export default AddTask;