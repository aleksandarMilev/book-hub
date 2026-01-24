import type { TFunction } from 'i18next';
import * as Yup from 'yup';

const constraints = {
  content: { min: 10, max: 5_000 },
  rating: { min: 1, max: 5 },
};

export const reviewSchema = (t: TFunction<'reviews'>) =>
  Yup.object({
    content: Yup.string()
      .min(
        constraints.content.min,
        t('validation.min', {
          field: t('validation.fields.review'),
          min: constraints.content.min,
        }),
      )
      .max(
        constraints.content.max,
        t('validation.max', {
          field: t('validation.fields.review'),
          max: constraints.content.max,
        }),
      )
      .required(
        t('validation.required', {
          field: t('validation.fields.reviewContent'),
        }),
      ),
    rating: Yup.number()
      .min(constraints.rating.min, t('validation.rating.select'))
      .max(constraints.rating.max, t('validation.rating.max'))
      .required(t('validation.rating.select')),
  });
