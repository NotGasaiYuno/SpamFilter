﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MailService
{
    public class LikelihoodFunction
    {
        private readonly ModelResult<IEmailCategory> _bernoulli;
        private readonly ModelResult<IEmailCategory> _polynomial;

        public LikelihoodFunction(IEnumerable<ModelResult<IEmailCategory>> bernoulliResult,
            IEnumerable<ModelResult<IEmailCategory>> polynomialResult)
        {
            _bernoulli = GetResult(bernoulliResult);
            _polynomial = GetResult(polynomialResult);
        }

        private ModelResult<IEmailCategory> GetResult(IEnumerable<ModelResult<IEmailCategory>> results)
        {
            results = results.OrderByDescending(result => result.Probability);

            return results.First();
        }

        private int PseudorandomNumber(ValueType from, ValueType to)
        {
            Random random = new Random();

            return random.Next((int)from, (int)to);
        } 

        private double Approximation(double probability, double number) 
            => Math.Abs(probability - number);

        private string GetPseudorandomCategory()
        {
            string emailCategory;

            double number = PseudorandomNumber(_bernoulli.Probability, _polynomial.Probability);

            if (Approximation(_bernoulli.Probability, number)
                > Approximation(_polynomial.Probability, number))
            {
                emailCategory = _bernoulli.Category;
            }
            else
            {
                emailCategory = _polynomial.Category;
            }

            return emailCategory;
        }

        public string ClassifyEmail()
        {
            string emailCategory;

            if (_bernoulli.Category == _polynomial.Category)
            {
                emailCategory = _bernoulli.Category;
            }
            else
            {
                emailCategory = GetPseudorandomCategory();
            }

            return emailCategory;
        }
    }
}