import './ChatForm.css';

import type { FC } from 'react';

import image from '@/chat.avif';

import { useChatFormik } from './formik/useChatFormik';

const ChatForm: FC<{ chatData?: any; isEditMode?: boolean }> = ({
  chatData = null,
  isEditMode = false,
}) => {
  const formik = useChatFormik({ chatData, isEditMode });

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
            <label htmlFor="imageUrl">Image URL</label>
            <input
              id="imageUrl"
              name="imageUrl"
              placeholder="Enter chat image URL (optional)"
              value={formik.values.imageUrl ?? ''}
              onChange={(e) => formik.setFieldValue('imageUrl', e.target.value || null)}
              onBlur={formik.handleBlur}
              className={`form-input ${formik.touched.imageUrl && formik.errors.imageUrl ? 'invalid' : ''}`}
            />
            {formik.touched.imageUrl && formik.errors.imageUrl && (
              <div className="error-message">{formik.errors.imageUrl}</div>
            )}
          </div>
          <button type="submit" className="form-submit-btn">
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
