﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevIO.Api.Controller;
using DevIO.Api.Extensions;
using DevIO.Api.ViewModels;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.Api.V1.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/fornecedores")]
    public class FornecedoresController : MainController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedorRepository fornecedorRepository
                                    , IMapper mapper
                                    , IFornecedorService fornecedorService
                                    , IEnderecoRepository enderecoRepository
                                    , INotificador notificador
                                    , IUser user) : base(notificador, user)
        {
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
            _fornecedorService = fornecedorService;
            _enderecoRepository = enderecoRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<FornecedorViewModel>> ObterTodos()
        {
            //var fornecedor = _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterTodos());
            //return fornecedor;
            return _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> ObterPorId(Guid id)
        {
            var fornecedor = await ObterFornecedorProdutosEndereco(id);

            if (fornecedor == null) return NotFound();

            return fornecedor;
        }

        [ClaimsAuthorize("Fornecedor", "Adicionar")]
        [HttpPost]
        public async Task<ActionResult<FornecedorViewModel>> Adicionar(FornecedorViewModel fornecedorViewModel)
        {
            //Opção sem a implementação do IUser na MainController
            //if (User.Identity.IsAuthenticated)
            //{
            //    var userName = User.Identity.Name;
            //}

            if (UsuarioAutenticado)
            {
                var userId = UsuarioId;
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            await _fornecedorService.Adicionar(fornecedor);
            
            return CustomResponse(fornecedorViewModel);
        }

        [ClaimsAuthorize("Fornecedor", "Atualizar")]
        [ClaimsAuthorize("Swagger", "Atualizar")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Atualizar(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id)
            {
                NotificarErro("O ID informado não é o mesmo que foi passado na query");
                return CustomResponse(fornecedorViewModel);
            }

            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);

            await _fornecedorService.Atualizar(fornecedor);
            
            return CustomResponse(fornecedorViewModel);
        }
        
        [ClaimsAuthorize("Fornecedor", "Remover")]
        [ClaimsAuthorize("Swagger", "Remover")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Excluir(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null) return NotFound();

            await _fornecedorService.Remover(id);
            
            return CustomResponse();
        }


        [HttpGet("endereco/{id:guid}")]
        public async Task<EnderecoViewModel> ObterEnderecoPorId(Guid id)
        {
            return _mapper.Map<EnderecoViewModel>(await _enderecoRepository.ObterPorId(id));
        }

        [ClaimsAuthorize("Fornecedor", "Atualizar")]
        [HttpPut("endereco/{id:guid}")]
        public async Task<IActionResult> AtualizarEndereco(Guid id, EnderecoViewModel enderecoViewModel)
        {
            if (id != enderecoViewModel.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(enderecoViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(enderecoViewModel));

            return CustomResponse(enderecoViewModel);
        }



        private async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
        }

        private async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
        }
    }
}