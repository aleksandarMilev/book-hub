import './Message.css'

export default function MessageDisplay({ message, isSuccess }) {
    return (
        <div className={`message ${isSuccess ? 'success' : 'error'}`}>
            <p>{message}</p>
        </div>
    )
}
