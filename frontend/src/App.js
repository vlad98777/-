import React, { useState, useEffect } from 'react';
import axios from 'axios';

function App() {
  const [books, setBooks] = useState([]);
  const [newBook, setNewBook] = useState({
    title: '',
    author: '',
    year: new Date().getFullYear(),
    genre: 'Роман',
    isAvailable: true
  });
  const [genres] = useState(['Роман', 'Фантастика', 'Фэнтези', 'Детектив', 'Антиутопия', 'Научная литература']);

  useEffect(() => {
    fetchBooks();
  }, []);

  const fetchBooks = async () => {
    try {
      const response = await axios.get('http://localhost:5000/api/books');
      setBooks(response.data);
    } catch (error) {
      console.error('Error fetching books:', error);
    }
  };

  const addBook = async () => {
    if (!newBook.title.trim() || !newBook.author.trim()) return;
    
    try {
      await axios.post('http://localhost:5000/api/books', newBook);
      setNewBook({
        title: '',
        author: '',
        year: new Date().getFullYear(),
        genre: 'Роман',
        isAvailable: true
      });
      fetchBooks();
    } catch (error) {
      console.error('Error adding book:', error);
    }
  };

  const deleteBook = async (id) => {
    try {
      await axios.delete(`http://localhost:5000/api/books/${id}`);
      fetchBooks();
    } catch (error) {
      console.error('Error deleting book:', error);
    }
  };

  const toggleAvailability = async (book) => {
    try {
      await axios.put(`http://localhost:5000/api/books/${book.id}`, {
        ...book,
        isAvailable: !book.isAvailable
      });
      fetchBooks();
    } catch (error) {
      console.error('Error updating book:', error);
    }
  };

  return (
    <div style={{ padding: '20px', maxWidth: '800px', margin: '0 auto' }}>
      <h1>📚 Библиотека книг</h1>
      
      {/* Форма добавления книги */}
      <div style={{ 
        backgroundColor: '#f5f5f5', 
        padding: '20px', 
        borderRadius: '8px',
        marginBottom: '20px'
      }}>
        <h3>Добавить новую книгу</h3>
        <div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
          <input
            type="text"
            placeholder="Название книги"
            value={newBook.title}
            onChange={(e) => setNewBook({...newBook, title: e.target.value})}
            style={{ padding: '8px' }}
          />
          <input
            type="text"
            placeholder="Автор"
            value={newBook.author}
            onChange={(e) => setNewBook({...newBook, author: e.target.value})}
            style={{ padding: '8px' }}
          />
          <div style={{ display: 'flex', gap: '10px' }}>
            <input
              type="number"
              placeholder="Год издания"
              value={newBook.year}
              onChange={(e) => setNewBook({...newBook, year: parseInt(e.target.value) || new Date().getFullYear()})}
              style={{ padding: '8px', flex: 1 }}
            />
            <select
              value={newBook.genre}
              onChange={(e) => setNewBook({...newBook, genre: e.target.value})}
              style={{ padding: '8px', flex: 1 }}
            >
              {genres.map(genre => (
                <option key={genre} value={genre}>{genre}</option>
              ))}
            </select>
          </div>
          <button 
            onClick={addBook}
            style={{ 
              padding: '10px', 
              backgroundColor: '#4CAF50', 
              color: 'white', 
              border: 'none',
              borderRadius: '4px',
              cursor: 'pointer'
            }}
          >
            Добавить книгу
          </button>
        </div>
      </div>

      {/* Список книг */}
      <div>
        <h3>Книги в библиотеке ({books.length})</h3>
        {books.length === 0 ? (
          <p>Нет книг в библиотеке</p>
        ) : (
          <div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
            {books.map(book => (
              <div 
                key={book.id}
                style={{ 
                  border: '1px solid #ddd',
                  borderRadius: '8px',
                  padding: '15px',
                  backgroundColor: book.isAvailable ? '#f9f9f9' : '#fff5f5'
                }}
              >
                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
                  <div style={{ flex: 1 }}>
                    <h4 style={{ margin: '0 0 5px 0' }}>
                      {book.title} 
                      <span style={{ 
                        fontSize: '12px', 
                        backgroundColor: book.isAvailable ? '#4CAF50' : '#f44336',
                        color: 'white',
                        padding: '2px 8px',
                        borderRadius: '12px',
                        marginLeft: '10px'
                      }}>
                        {book.isAvailable ? 'Доступна' : 'Выдана'}
                      </span>
                    </h4>
                    <p style={{ margin: '0 0 5px 0', color: '#666' }}>
                      <strong>Автор:</strong> {book.author}
                    </p>
                    <p style={{ margin: '0 0 5px 0', color: '#666' }}>
                      <strong>Год:</strong> {book.year} | <strong>Жанр:</strong> {book.genre}
                    </p>
                    <small style={{ color: '#999' }}>
                      Добавлено: {new Date(book.addedDate).toLocaleDateString()}
                    </small>
                  </div>
                  <div style={{ display: 'flex', gap: '10px' }}>
                    <button
                      onClick={() => toggleAvailability(book)}
                      style={{
                        padding: '5px 10px',
                        backgroundColor: book.isAvailable ? '#f44336' : '#4CAF50',
                        color: 'white',
                        border: 'none',
                        borderRadius: '4px',
                        cursor: 'pointer'
                      }}
                    >
                      {book.isAvailable ? 'Выдать' : 'Вернуть'}
                    </button>
                    <button
                      onClick={() => deleteBook(book.id)}
                      style={{
                        padding: '5px 10px',
                        backgroundColor: '#ff9800',
                        color: 'white',
                        border: 'none',
                        borderRadius: '4px',
                        cursor: 'pointer'
                      }}
                    >
                      Удалить
                    </button>
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>

      {/* Статистика */}
      <div style={{ marginTop: '20px', padding: '10px', backgroundColor: '#e8f5e8', borderRadius: '8px' }}>
        <h4>📊 Статистика библиотеки</h4>
        <p>Всего книг: {books.length}</p>
        <p>Доступно: {books.filter(b => b.isAvailable).length}</p>
        <p>Выдано: {books.filter(b => !b.isAvailable).length}</p>
      </div>
    </div>
  );
}

export default App;
