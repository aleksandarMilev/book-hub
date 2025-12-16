import './ChatForm.css';

import type { FC } from 'react';
import { useRef } from 'react';

import image from '@/chat.avif';
import type { Chat } from '@/features/chat/types/chat.js';

import { useChatFormik } from './formik/useChatFormik.js';

type Props = { chatData?: Chat; isEditMode?: boolean };

const ChatForm: FC<Props> = ({ chatData = null, isEditMode = false }) => {
  const formik = useChatFormik({ chatData, isEditMode });
  const fileInputRef = useRef<HTMLInputElement | null>(null);

  return (
    <div className="chat-form-container">
      <div className="chat-form-card">
        <div className="chat-form-image">
          <img src={image} alt="Chat Illustration" />
        </div>
        <h2 className="chat-form-title">{isEditMode ? 'Edit Chat' : 'Create New Chat'}</h2>
        <form className="chat-form" onSubmit={formik.handleSubmit}>
          <div className="form-group">
            <label htmlFor="name">Name</label>
            <input
              id="name"
              name="name"
              placeholder="Enter chat name"
              value={formik.values.name}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              className={`form-input ${formik.touched.name && formik.errors.name ? 'invalid' : ''}`}
            />
            {formik.touched.name && formik.errors.name && (
              <div className="error-message">{formik.errors.name}</div>
            )}
          </div>
          <div className="form-group">
            <label htmlFor="image">Chat image (optional)</label>
            <input
              ref={fileInputRef}
              id="image"
              name="image"
              type="file"
              accept="image/*"
              onChange={(e) => {
                const file = e.currentTarget.files?.[0] ?? null;
                formik.setFieldValue('image', file);
              }}
              onBlur={formik.handleBlur}
              className={`form-input ${formik.touched.image && formik.errors.image ? 'invalid' : ''}`}
            />
            {formik.touched.image && formik.errors.image && (
              <div className="error-message">{formik.errors.image as string}</div>
            )}
            {formik.values.image && (
              <div className="hint">
                Selected: <strong>{formik.values.image.name}</strong>
                <button
                  type="button"
                  className="link-button"
                  onClick={() => {
                    formik.setFieldValue('image', null);
                    if (fileInputRef.current) {
                      fileInputRef.current.value = '';
                    }
                  }}
                >
                  Remove
                </button>
              </div>
            )}
            {isEditMode && chatData?.imagePath && (
              <div className="hint">
                Current image path: <strong>{chatData.imagePath}</strong>
              </div>
            )}
          </div>
          <button type="submit" className="form-submit-btn" disabled={formik.isSubmitting}>
            {formik.isSubmitting
              ? isEditMode
                ? 'Saving...'
                : 'Creating...'
              : isEditMode
                ? 'Update Chat'
                : 'Create Chat'}
          </button>
        </form>
      </div>
    </div>
  );
};

export default ChatForm;
