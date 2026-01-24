import './ChatForm.css';

import type { FC } from 'react';
import { useRef } from 'react';
import { useTranslation } from 'react-i18next';

import image from './assets/chat.avif';
import type { Chat } from '@/features/chat/types/chat.js';

import { useChatFormik } from './formik/useChatFormik.js';

type Props = { chatData?: Chat; isEditMode?: boolean };

const ChatForm: FC<Props> = ({ chatData = null, isEditMode = false }) => {
  const { t } = useTranslation('chats');

  const formik = useChatFormik({ chatData, isEditMode });
  const fileInputRef = useRef<HTMLInputElement | null>(null);

  return (
    <div className="chat-form-container">
      <div className="chat-form-card">
        <div className="chat-form-image">
          <img src={image} alt={t('details.chatImageAlt')} />
        </div>
        <h2 className="chat-form-title">
          {isEditMode ? t('form.titleEdit') : t('form.titleCreate')}
        </h2>
        <form className="chat-form" onSubmit={formik.handleSubmit}>
          <div className="form-group">
            <label htmlFor="name">{t('form.labels.name')}</label>
            <input
              id="name"
              name="name"
              placeholder={t('form.placeholders.name')}
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
            <label htmlFor="image">{t('form.labels.image')}</label>
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
                {t('form.image.selected', { fileName: formik.values.image.name })}{' '}
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
                  {t('form.image.remove')}
                </button>
              </div>
            )}

            {isEditMode && chatData?.imagePath && (
              <div className="hint">
                {t('form.image.currentPath', { path: chatData.imagePath })}
              </div>
            )}
          </div>

          <button type="submit" className="form-submit-btn" disabled={formik.isSubmitting}>
            {formik.isSubmitting
              ? isEditMode
                ? t('form.buttons.saving')
                : t('form.buttons.creating')
              : isEditMode
                ? t('form.buttons.submitEdit')
                : t('form.buttons.submitCreate')}
          </button>
        </form>
      </div>
    </div>
  );
};

export default ChatForm;
